# Tarsier.ShareScreen
<p align="center">
  <a href="https://github.com/marianz-bonfire/Tarsier.ShareScreen">
    <img height="250" src="https://raw.githubusercontent.com/marianz-bonfire/tarsier_assets/master/package-assets/tarsier_sharescreen/tarsier.share250.png">
  </a>
  <h1 align="center">Tarsier ShareScreen</h1>
</p>
A powerful Windows desktop application for streaming screen, monitors, application windows, and webcams over the network via MJPEG (Motion JPEG) protocol.

## Features

- **Multi-Source Streaming**: Stream from multiple sources including:
  - Desktop Screen (Primary Monitor)
  - Individual Monitors (Multi-monitor support)
  - Application Windows (including browser tabs)
  - Webcams (with device selection for multiple cameras)
  
- **Configurable Stream Quality**:
  - Adjustable screen resolution (480p, 720p, 1080p, 1440p, 4K)
  - Selectable frame rate (5, 10, 15, 20, 24, 30, 60 FPS)
  - Real-time compression with MJPEG encoding

- **Network Streaming**:
  - Standard HTTP MJPEG stream protocol
  - Browser-compatible (works with any modern web browser)
  - Multiple simultaneous client connections supported
  - Auto-detected server IP and port

- **User-Friendly (DEMO) Interface**:
  - Simple WinForms GUI with source selection
  - Live display of server URL for easy sharing
  - One-click copy and browser open functionality
  - Device/application refresh buttons for dynamic selection


### Demo

<img src="https://raw.githubusercontent.com/marianz-bonfire/tarsier_assets/master/package-assets/tarsier_sharescreen/demo.gif">

## System Requirements

- **Operating System**: Windows 7 or later
- **Framework**: .NET Framework 4.7.2
- **.NET Runtime**: Should be pre-installed on Windows 10/11
- **Hardware**: 
  - Multi-core processor recommended
  - Sufficient network bandwidth (varies by resolution/FPS)
- **Webcam** (optional): USB webcam or integrated camera for webcam streaming

## Installation

1. **Prerequisites**: Ensure .NET Framework 4.7.2 is installed on your system
   - Download from [Microsoft .NET Framework](https://dotnet.microsoft.com/download/dotnet-framework)

2. **Build from Source**:
   ```bash
   git clone https://github.com/yourusername/Tarsier.ShareScreen.git
   cd Tarsier.ShareScreen
   msbuild Tarsier.ShareScreen.sln /p:Configuration=Release
   ```

3. **Run the Application**:
   ```bash
   .\Tarsier.ShareScreen\bin\Release\Tarsier.ShareScreen.exe
   ```

## Usage Guide

### Basic Setup

1. **Launch the Application**
   - Run `Tarsier.ShareScreen.exe`
   - The main window displays stream configuration options

2. **Select Streaming Source**
   - Choose from the **Source Type** dropdown:
     - **Desktop Screen (Primary)**: Streams your primary monitor
     - **Monitor**: Select a specific monitor from the dropdown (ideal for multi-monitor setups)
     - **Application Window**: Select a running application window (includes browser windows for streaming browser tabs)
     - **Webcam**: Select a camera device from the dropdown (shows all available webcams)

3. **Configure Stream Quality**
   - **Resolution**: Select target resolution (480p, 720p, 1080p, 1440p, 4K)
   - **FPS**: Choose frame rate (5-60 FPS)
   - **Show Cursor**: Check to include mouse cursor in the stream (not available for webcams)

4. **Refresh Device Lists** (as needed)
   - **Monitors**: Click "Refresh" to detect newly connected displays
   - **Webcams**: Click "Refresh" to detect newly connected cameras

5. **Start Streaming**
   - Click the **Start Streaming** button
   - The server URL appears in the text field below
   - Status label confirms streaming is active

### Accessing the Stream

**From the Same Machine**:
```
http://localhost:<port>
```

**From Another Machine on the Network**:
```
http://<server-ip>:<port>
```

- Example: `http://192.168.1.100:7777`
- The application auto-detects and displays the correct URL
- Use the **Copy** button to copy the URL to clipboard
- Use the **Open in Browser** button for quick access

**Supported Browsers**:
- Chrome
- Firefox
- Edge
- Safari
- Any browser supporting HTTP MJPEG streams

### Example Scenarios

**Scenario 1: Present Your Screen in a Meeting**
1. Select "Desktop Screen (Primary)" or specific "Monitor"
2. Set Resolution to 1080p, FPS to 30
3. Start Streaming
4. Share the URL with meeting attendees
5. They open it in their browser to see your screen

**Scenario 2: Stream a Browser Window**
1. Ensure your browser window is visible and active
2. Select "Application Window" from Source Type
3. Choose your browser from the Applications list
4. Start Streaming
5. Attendees see your browser tab content

**Scenario 3: Stream Multiple Webcams**
1. Connect additional USB webcams (if available)
2. For each camera:
   - Select "Webcam" as Source Type
   - Choose device from the dropdown
   - Click "Refresh Webcams" if new device isn't visible
   - Start Streaming (restart to switch between cameras)

## Architecture

### Components

- **Tarsier.ShareScreen.Core**: Core streaming engine
  - MJPEG encoding and HTTP server
  - Screenshot capture (desktop, monitor, window, webcam)
  - Streaming source management

- **Tarsier.ShareScreen.DirectX.Capture**: DirectShow wrapper
  - Low-level DirectX video capture
  - Filter graph management

- **Tarsier.ShareScreen.DShowNET**: COM interop for DirectShow
  - Direct Show interfaces and enumerations
  - DirectShow device management

- **Tarsier.ShareScreen**: WinForms UI
  - User interface and configuration
  - Streaming server management

### Streaming Protocol

- **Format**: MJPEG (Motion JPEG) over HTTP
- **Mime Type**: `multipart/x-mixed-replace`
- **Connection Type**: Keep-alive HTTP
- **Encoding**: Individual frames as JPEG images
- **Scalability**: Supports multiple simultaneous clients

## Configuration

### Network Settings

The application listens on:
- **IP**: 0.0.0.0 (all available network interfaces)
- **Default Port**: 7777 (can be modified in source code)

### Performance Tuning

- **Lower Resolution + Lower FPS**: Better for low-bandwidth networks
- **Higher Resolution + Higher FPS**: Better for local networks (LAN)
- **Show Cursor**: Slightly increases CPU usage due to cursor overlay rendering

## Troubleshooting

### Issue: "No monitors detected"
- **Solution**: Click "Refresh Monitors" button to re-enumerate connected displays

### Issue: "No webcams detected"
- **Solution**: 
  - Verify camera is connected and recognized in Device Manager
  - Click "Refresh Webcams" button
  - Check that camera is not in use by another application

### Issue: "Stream URL shows localhost instead of network IP"
- **Solution**: The app auto-detects the primary network interface; access from other machines using the displayed IP address

### Issue: "Stream is laggy or slow"
- **Solution**:
  - Reduce resolution or FPS
  - Check network bandwidth availability
  - Verify no other bandwidth-heavy applications are running

### Issue: "Cannot connect from another machine"
- **Solution**:
  - Verify both machines are on the same network
  - Check Windows Firewall allows the port (default: 7777)
  - Confirm the IP address matches your network (not localhost/127.0.0.1)

## License

This project is licensed under the License included in the LICENSE file.

## Contributing

Contributions are welcome! Please feel free to submit pull requests or report issues.

## Technical Stack

- **Language**: C#
- **Framework**: .NET Framework 4.7.2
- **UI**: Windows Forms (WinForms)
- **Video Capture**: DirectShow / DirectX
- **Protocol**: HTTP MJPEG
- **Threading**: Managed .NET threading with async operations
