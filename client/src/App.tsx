import React, { useState, useEffect } from 'react';
import axios from 'axios';
import './App.css';

interface Message {
  id: string;
  text: string;
  isUser: boolean;
  timestamp: Date;
}

const API_URL = process.env.REACT_APP_API_URL || 'http://localhost:5000';

function App() {
  const [messages, setMessages] = useState<Message[]>([]);
  const [input, setInput] = useState('');
  const [loading, setLoading] = useState(false);
  const [user, setUser] = useState<any>(null);
  const [showAuth, setShowAuth] = useState(false);

  useEffect(() => {
    // 检查用户是否已登录
    const token = localStorage.getItem('token');
    if (token) {
      setUser({ token });
    }
  }, []);

  const handleSendMessage = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!input.trim()) return;

    const userMessage: Message = {
      id: Date.now().toString(),
      text: input,
      isUser: true,
      timestamp: new Date(),
    };

    setMessages((prev) => [...prev, userMessage]);
    setInput('');
    setLoading(true);

    try {
      const response = await axios.post(
        `${API_URL}/api/chat/message`,
        { message: input },
        {
          headers: {
            Authorization: `Bearer ${user?.token}`,
          },
        }
      );

      const aiMessage: Message = {
        id: (Date.now() + 1).toString(),
        text: response.data.reply,
        isUser: false,
        timestamp: new Date(),
      };

      setMessages((prev) => [...prev, aiMessage]);
    } catch (error) {
      console.error('Error sending message:', error);
      const errorMessage: Message = {
        id: (Date.now() + 1).toString(),
        text: '抱歉，发生了错误。请稍后重试。',
        isUser: false,
        timestamp: new Date(),
      };
      setMessages((prev) => [...prev, errorMessage]);
    } finally {
      setLoading(false);
    }
  };

  if (!user) {
    return (
      <div className="auth-container">
        <div className="auth-box">
          <h1>🤖 AI 智能助手</h1>
          <p>欢迎使用我们的AI助手平台</p>
          <button onClick={() => setShowAuth(!showAuth)} className="auth-button">
            {showAuth ? '返回' : '开始使用'}
          </button>
        </div>
      </div>
    );
  }

  return (
    <div className="app-container">
      <header className="app-header">
        <h1>🤖 AI 智能助手</h1>
        <button
          onClick={() => {
            localStorage.removeItem('token');
            setUser(null);
          }}
          className="logout-button"
        >
          登出
        </button>
      </header>

      <div className="chat-container">
        <div className="messages">
          {messages.length === 0 ? (
            <div className="empty-state">
              <p>👋 开始对话，体验AI的力量</p>
              <p className="hint">您可以询问任何问题...</p>
            </div>
          ) : (
            messages.map((message) => (
              <div
                key={message.id}
                className={`message ${message.isUser ? 'user' : 'ai'}`}
              >
                <div className="message-content">{message.text}</div>
              </div>
            ))
          )}
          {loading && (
            <div className="message ai">
              <div className="message-content">思考中...</div>
            </div>
          )}
        </div>

        <form onSubmit={handleSendMessage} className="input-form">
          <input
            type="text"
            value={input}
            onChange={(e) => setInput(e.target.value)}
            placeholder="输入您的问题..."
            disabled={loading}
            className="input-field"
          />
          <button type="submit" disabled={loading} className="send-button">
            {loading ? '发送中...' : '发送'}
          </button>
        </form>
      </div>
    </div>
  );
}

export default App;
