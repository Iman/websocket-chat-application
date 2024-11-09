import React, { useState, useEffect, useRef } from 'react';

const Chat = () => {
    const [clientId] = useState(Date.now());
    const [messages, setMessages] = useState([]);
    const [input, setInput] = useState('');
    const ws = useRef(null);

    useEffect(() => {

        ws.current = new WebSocket(`ws://localhost:8000/ws/${clientId}`);

        ws.current.onopen = () => {
            console.log('Connected to WebSocket server');
        };

        ws.current.onmessage = (event) => {
            setMessages(prevMessages => [...prevMessages, event.data]);
        };

        ws.current.onclose = () => {
            console.log('Disconnected from WebSocket server');
        };

        return () => {
            ws.current.close();
        };
    }, [clientId]);

    const handleSendMessage = (event) => {
        event.preventDefault();
        if (input) {
            ws.current.send(input);
            setInput('');
        }
    };

    return (
        <div style={{ padding: '20px' }}>
            <h1>WebSocket Chat</h1>
            <h2>Your ID: {clientId}</h2>
            <form onSubmit={handleSendMessage}>
                <input
                    type="text"
                    value={input}
                    onChange={(e) => setInput(e.target.value)}
                    placeholder="Type a message..."
                    style={{ padding: '10px', width: '300px' }}
                />
                <button type="submit" style={{ padding: '10px' }}>Send</button>
            </form>
            <ul style={{ listStyleType: 'none', padding: 0, marginTop: '20px' }}>
                {messages.map((msg, index) => (
                    <li key={index} style={{ padding: '5px 0' }}>{msg}</li>
                ))}
            </ul>
        </div>
    );
};

export default Chat;
