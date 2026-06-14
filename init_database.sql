-- ======================================================
-- DATABASE: DoggyServer
-- Purpose: Store bot accounts, behaviors, and logs for Second Life Dog Bot
-- Created: Based on project analysis from 2026-06-14
-- ======================================================

-- NOT TESTED -> just ask an AI

-- 1. Table: Bot Accounts (Second Life credentials)
CREATE TABLE bot_accounts (
    id SERIAL PRIMARY KEY,
    username VARCHAR(100) NOT NULL UNIQUE,
    password_hash VARCHAR(255) NOT NULL, -- Hashed and salted!
    last_login TIMESTAMP,
    login_count INT DEFAULT 0,
    is_active BOOLEAN DEFAULT true,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- 2. Table: Bot Configuration (behavior, properties)
CREATE TABLE bot_configs (
    id SERIAL PRIMARY KEY,
    bot_account_id INT NOT NULL REFERENCES bot_accounts(id) ON DELETE CASCADE,
    bot_name VARCHAR(100),
    follow_distance FLOAT DEFAULT 5.0,
    animation_set VARCHAR(50) DEFAULT 'dog_default',
    behavior_profile VARCHAR(50) DEFAULT 'friendly',
    movement_speed FLOAT DEFAULT 2.5,
    is_following_enabled BOOLEAN DEFAULT true,
    config_json TEXT, -- For additional, non-normalized settings
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- 3. Table: Target Avatars (avatars the bot should follow)
CREATE TABLE target_avatars (
    id SERIAL PRIMARY KEY,
    bot_account_id INT NOT NULL REFERENCES bot_accounts(id) ON DELETE CASCADE,
    avatar_uuid VARCHAR(36) NOT NULL, -- UUID from Second Life
    avatar_name VARCHAR(100),
    priority INT DEFAULT 1, -- 1 = highest priority
    is_active BOOLEAN DEFAULT true,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UNIQUE(bot_account_id, avatar_uuid)
);

-- 4. Table: Behavior Logs (for debugging & analysis)
CREATE TABLE behavior_logs (
    id SERIAL PRIMARY KEY,
    bot_account_id INT NOT NULL REFERENCES bot_accounts(id) ON DELETE CASCADE,
    action_type VARCHAR(50), -- e.g., 'follow', 'bark', 'sit', 'move'
    target_avatar_uuid VARCHAR(36),
    details TEXT,
    log_level VARCHAR(20) DEFAULT 'INFO', -- INFO, WARN, ERROR, DEBUG
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- 5. Table: Movement Commands (queue for Movement System)
CREATE TABLE movement_commands (
    id SERIAL PRIMARY KEY,
    bot_account_id INT NOT NULL REFERENCES bot_accounts(id) ON DELETE CASCADE,
    command_type VARCHAR(50), -- e.g., 'goto', 'follow', 'stop'
    target_position_x FLOAT,
    target_position_y FLOAT,
    target_position_z FLOAT,
    target_avatar_uuid VARCHAR(36),
    status VARCHAR(20) DEFAULT 'pending', -- pending, in_progress, completed, failed
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    processed_at TIMESTAMP
);

-- 6. Table: Inventory Items (manages virtual objects)
CREATE TABLE inventory_items (
    id SERIAL PRIMARY KEY,
    bot_account_id INT NOT NULL REFERENCES bot_accounts(id) ON DELETE CASCADE,
    item_name VARCHAR(255),
    item_uuid VARCHAR(36),
    item_type VARCHAR(50), -- animation, sound, object, etc.
    is_equipped BOOLEAN DEFAULT false,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- 7. Table: Session Data (for active bot sessions)
CREATE TABLE bot_sessions (
    id SERIAL PRIMARY KEY,
    bot_account_id INT NOT NULL REFERENCES bot_accounts(id) ON DELETE CASCADE,
    session_token VARCHAR(255) UNIQUE NOT NULL,
    ip_address INET,
    last_activity TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    is_valid BOOLEAN DEFAULT true,
    expires_at TIMESTAMP DEFAULT (CURRENT_TIMESTAMP + INTERVAL '24 hours'),
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- ======================================================
-- INDEXES for Performance
-- ======================================================
CREATE INDEX idx_bot_accounts_username ON bot_accounts(username);
CREATE INDEX idx_target_avatars_bot_id ON target_avatars(bot_account_id);
CREATE INDEX idx_behavior_logs_bot_id ON behavior_logs(bot_account_id);
CREATE INDEX idx_behavior_logs_created_at ON behavior_logs(created_at);
CREATE INDEX idx_movement_commands_bot_id ON movement_commands(bot_account_id);
CREATE INDEX idx_movement_commands_status ON movement_commands(status);
CREATE INDEX idx_bot_sessions_token ON bot_sessions(session_token);
CREATE INDEX idx_bot_sessions_bot_id ON bot_sessions(bot_account_id);

-- ======================================================
-- VIEWS for frequently used queries
-- ======================================================
CREATE VIEW active_bots_with_config AS
SELECT 
    ba.id, ba.username, ba.is_active,
    bc.bot_name, bc.follow_distance, bc.behavior_profile,
    bc.is_following_enabled
FROM bot_accounts ba
LEFT JOIN bot_configs bc ON ba.id = bc.bot_account_id
WHERE ba.is_active = true;

-- ======================================================
-- TRIGGER for updated_at (automatic timestamps)
-- ======================================================
CREATE OR REPLACE FUNCTION update_updated_at_column()
RETURNS TRIGGER AS $$
BEGIN
    NEW.updated_at = CURRENT_TIMESTAMP;
    RETURN NEW;
END;
$$ language 'plpgsql';

CREATE TRIGGER update_bot_accounts_updated_at 
    BEFORE UPDATE ON bot_accounts 
    FOR EACH ROW EXECUTE FUNCTION update_updated_at_column();