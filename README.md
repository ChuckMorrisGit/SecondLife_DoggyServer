# DoggyServer - Second Life Companion Bot

## Overview

DoggyServer is a .NET-based application that simulates an intelligent dog companion bot for the Second Life virtual world. The bot can follow avatars, perform various dog-like behaviors, and interact with the Second Life environment through direct communication with the Linden Lab servers.

This project was started in 2010 and actively developed and maintained until 2013. It was primarily developed as a research and testing platform to explore various communication protocols and functionalities with the Second Life servers.

## ⚠️ Important Legal Notice

**DISCLAIMER: This project contains code and functionalities that violate the Terms of Service of Linden Lab and Second Life. Use of this application for any production, testing, or deployment purposes is strongly discouraged and may result in account suspension or permanent banning from Second Life.**

Some features implemented in this project circumvent Second Life security measures and violate the official Terms of Service. Users are advised to use this code only for educational and research purposes, and only in isolated, non-production environments.

## Features

- **Avatar Following**: The bot can detect and follow designated avatars
- **Behavioral Simulation**: Implements dog-like behaviors and animations
- **Multi-threaded Server Architecture**: Handles multiple simultaneous connections
- **Second Life Protocol Support**: Direct communication with Linden Lab servers
- **Customizable Configuration**: Settings and parameters for bot behavior
- **Logging and Debugging**: Comprehensive logging for troubleshooting

## Project Structure

- **DoggyServer/**: Main server application containing the core bot logic
  - Communication with Second Life servers
  - Avatar tracking and following
  - Behavior simulation and animation management
  - Database and account management
  - Inventory and asset handling
  
- **DoggyClient/**: Client application for bot management and testing
- **DoggyMCP/**: MCP (Metaverse Communications Protocol) implementation layer (I called it "Master Control Program")

## Technical Stack

- **Language**: C# (.NET Framework)
- **Architecture**: Multi-threaded server-client model
- **Configuration**: JSON-based settings management
- **Dependencies**: MQTT support for messaging, Microsoft Extensions for configuration and logging

## Components

### Core Modules

- **Accounts Manager**: Handles bot account creation and authentication
- **Avatar Management**: Tracks and manages avatar interactions
- **Movement System**: Controls bot navigation and following behavior
- **Communication Layer**: Direct protocol implementation for Second Life
- **Animation System**: Manages dog behaviors and animations
- **Inventory Management**: Handles virtual items and assets
- **Database Integration**: Persistent storage for bot state and data

## Compilation and Setup

This is a Visual Studio project. To compile:

1. Open `DoggyServer.sln` in Visual Studio
2. Restore NuGet packages
3. Build the solution (Note: Some dependencies use preview versions)
4. Configure `secrets.json` with required credentials (see `secrets.json_EXAMPLE`)
5. Mayby use init_database.sql for creating Databases/Tables. Not testet!!!!

## Security and Compliance Notes

- This project requires valid Second Life credentials to function
- Many core functionalities directly bypass or circumvent Second Life's official APIs
- The implementation includes undocumented protocol handling that violates Linden Lab's Terms of Service
- **Do not use with real Second Life accounts intended for legitimate gameplay**

## Educational Value

While this project is not suitable for production use, it serves as a valuable resource for understanding:
- Second Life's underlying network protocols
- Bot automation in virtual worlds
- Multi-threaded server architecture
- C# network programming
- Virtual world interaction design

## Limitations and Known Issues

- ToS violations make any real-world deployment impossible
- Designed primarily for research and educational testing
- Protocol implementations may be outdated or incomplete
- No warranty or support provided

## License

This project is provided as-is for educational and research purposes only. Users assume all responsibility for any violations of applicable terms of service or legal regulations.

## Disclaimer

Users of this code are solely responsible for ensuring their usage complies with:
- Linden Lab's Terms of Service
- Applicable local and international laws
- Any other relevant terms and conditions

The developers of this project cannot be held responsible for account suspension, banning, legal action, or any other consequences resulting from the use of this code.

---

**Last Updated**: June 2026 (Try to earse "private" Code)


