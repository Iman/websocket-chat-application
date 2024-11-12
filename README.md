
# WebSocket Chat Application

This is an example of full-stack WebSocket chat application built with FastAPI for the backend, React for the frontend, 
and a C# (.NET) WebSocket client running in a Docker container (cloud-native). The backend serves as a WebSocket server 
that allows clients to connect, send messages, and receive real-time broadcasts. The frontend and Dockerized C# client 
connect to this WebSocket server, enabling a live chat interface.

This project demonstrates:

1. Initiating a REST API Call as an entry point, the application starts with a RESTful call to initiate client 
connections, after which clients can seamlessly upgrade to a WebSocket connection. This pattern is commonly used to 
establish a bi-directional, asynchronous connection over HTTP/S. 
2. Multiduplex Broadcasting with FastAPI framework, the server supports real-time, multiduplex 
communication. This enables broadcast messaging to all connected clients, making it an ideal foundation for any 
application that requires bidirectional communication or live updates.

## Table of Contents

- [Project Structure](#project-structure)
- [Prerequisites](#prerequisites)
- [Backend Setup (FastAPI)](#backend-setup-fastapi)
  - [Install Dependencies](#install-dependencies)
  - [Run the FastAPI Server](#run-the-fastapi-server)
  - [WebSocket Endpoints](#websocket-endpoints)
  - [Broadcasting Messages](#broadcasting-messages)
- [Frontend Setup (React)](#frontend-setup-react)
  - [Install Dependencies](#install-dependencies-1)
  - [Run the React Application](#run-the-react-application)
  - [WebSocket Client Logic](#websocket-client-logic)
- [C# (.NET) WebSocket Client in Docker](#c-net-websocket-client-in-docker)
  - [Dockerfile Configuration](#dockerfile-configuration)
  - [Building and Running the Docker Container](#building-and-running-the-docker-container)
  - [Connecting with a Client ID](#connecting-with-a-client-id)
- [Docker Setup for FastAPI](#docker-setup-for-fastapi)
  - [Build the Docker Image](#build-the-docker-image)
  - [Run the Docker Container](#run-the-docker-container)
- [Integrating the Frontend and Backend](#integrating-the-frontend-and-backend)
  - [Initial API Call for Client ID](#initial-api-call-for-client-id)
  - [Using the Client ID in WebSocket Connections](#using-the-client-id-in-websocket-connections)
- [Testing](#testing)
- [Deploying to Production](#deploying-to-production)
- [References and Further Reading](#references-and-further-reading)

---

## Project Structure

An overview of the application's file structure, highlighting the backend, frontend, and Docker configurations.

```shell
root/
├── .venv/                    # Virtual environment for Python dependencies
├── UI/                       # Frontend directory for the React application
│   └── websocket-chat/       # React app created with create-react-app
├── ClientWebSocket.cs        # C# WebSocket client for Docker container
├── api.py                    # FastAPI server for handling WebSocket connections
├── Dockerfile                # Dockerfile for FastAPI server
├── README.md                 # Documentation for the project
└── package.json              # Node dependencies for the React app
```

## Prerequisites

- **Python 3.8+** – [Download here](https://www.python.org/downloads/)
- **Node.js and npm** – [Download here](https://nodejs.org/)
- **Docker** – [Download here](https://www.docker.com/get-started)
- **.NET SDK (for C# client)** – [Download here](https://dotnet.microsoft.com/download) (Optional)

## Backend Setup (FastAPI)

The backend is built with FastAPI, a modern, high-performance web framework ideal for asynchronous APIs and WebSocket handling. FastAPI is used to create RESTful endpoints and manage WebSocket connections for real-time data streaming and broadcast capabilities.

### 1. Install Dependencies

Set up a Python virtual environment (recommended) and install FastAPI and Uvicorn (ASGI server):

```bash
python3 -m venv .venv
source .venv/bin/activate  # For Linux/macOS
.venv\Scripts\activate     # For Windows

# Install FastAPI and Uvicorn
 pip install -r requirements.txt
```

### 2. Run the FastAPI Server
To start the FastAPI server, run the following command:

```shell
uvicorn api:app --host 0.0.0.0 --port 8000
```
Or

```shell
fastapi dev api.py 
```

The server will be available at `http://localhost:8000`.
The  Swagger UI documentation will be available at `http://localhost:8000/docs`, alternative API docs (ReDoc) `http://127.0.0.1:8000/redoc`.

**Note:** An HTML example is embedded to facilitate interaction and is ignored for readability during testing purposes.

## 3. WebSocket Endpoints
- GET `/`: This endpoint serves an HTML page with a WebSocket client interface, allowing quick testing of WebSocket functionality.
- GET `/ws/{client_id}`: This endpoint establishes a WebSocket connection, where `{client_id}` is a unique identifier for each client. This WebSocket endpoint supports full-duplex communication, allowing clients to send and receive messages in real-time.

### 4. Broadcasting Messages
The backend enables multiduplex communication where each client can send messages that are broadcast to all connected clients. Here’s how message broadcasting works:

- Personal Messages: Messages sent by a client are echoed back to that client as:
```plaintext
You wrote: {message}
```
- Broadcast Messages: Messages sent by a client are broadcasted to all other clients as:
```plaintext
Client #{client_id} says: {message}
```
Disconnect Notification: When a client disconnects, the following message is broadcasted to notify other clients:
```plaintext
Client #{client_id} left the chat
```

This approach makes FastAPI an effective choice for applications requiring bi-directional, async, multiduplex communication over HTTP/S.

---

## Frontend Setup (React)
The frontend is a React application that connects to the WebSocket server to enable real-time messaging. It demonstrates the concept of an initial REST call to retrieve the `client_id`, which is then used to establish a WebSocket connection.

### 1. Install Dependencies
Navigate to the React project directory `(UI/websocket-chat)` and install dependencies with:

```bash
cd UI/websocket-chat
npm install
```

### 2. Run the React Application
To start the React development server:

```bash
npm start
```
The React app will be available at `http://localhost:3000`.

### 3. WebSocket Client Logic
The React app initiates a WebSocket connection to the backend using `ws://localhost:8000/ws/{client_id}`. Each session generates a unique `client_id` upon loading the app.

Sending Messages: Users type messages into the input box, which are sent to the server.
Receiving Messages: Messages from other clients are displayed in real-time.

## References and Further Reading

- **FastAPI Documentation**: Learn more about FastAPI's features and capabilities for async APIs and WebSocket handling.  
  [FastAPI Documentation](https://fastapi.tiangolo.com/)

- **Uvicorn**: A lightning-fast ASGI server implementation, designed for Python async frameworks like FastAPI.  
  [Uvicorn on GitHub](https://github.com/encode/uvicorn)

- **WebSocket Protocol**: Understanding the WebSocket protocol for real-time, bidirectional communication over HTTP.  
  [WebSocket Protocol Specification](https://tools.ietf.org/html/rfc6455)

- **React Documentation**: Official React documentation for creating dynamic, component-based web applications.  
  [React Documentation](https://reactjs.org/docs/getting-started.html)

- **Docker Documentation**: Docker's official guide on containerizing applications for scalable deployment.  
  [Docker Documentation](https://docs.docker.com/)

- **.NET SDK**: Information and download page for the .NET SDK used for C# applications, including WebSocket clients.  
  [.NET SDK Download](https://dotnet.microsoft.com/download)
