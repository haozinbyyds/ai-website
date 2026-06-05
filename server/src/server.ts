import express, { Express, Request, Response } from 'express';
import cors from 'cors';
import dotenv from 'dotenv';

dotenv.config();

const app: Express = express();
const PORT = process.env.PORT || 5000;

// Middleware
app.use(cors());
app.use(express.json());

// Routes
app.get('/api/health', (req: Request, res: Response) => {
  res.json({ status: 'Server is running' });
});

app.post('/api/chat/message', (req: Request, res: Response) => {
  const { message } = req.body;
  
  // 简单的回复逻辑（实际应连接到AI API）
  const reply = `您说："${message}"。我是一个AI助手，正在学习中...`;
  
  res.json({ reply });
});

app.get('/api/chat/history', (req: Request, res: Response) => {
  res.json({ history: [] });
});

// Error handling
app.use((req: Request, res: Response) => {
  res.status(404).json({ message: 'Route not found' });
});

app.listen(PORT, () => {
  console.log(`✅ Server running on http://localhost:${PORT}`);
});
