# AI Website - 智能助手平台

一个现代化的AI助手网站，集成了智能聊天、文本生成等功能。

## ✨ 功能特性

- 🤖 AI智能聊天
- 📝 文本生成和优化
- 🎨 现代化UI设计
- 💾 对话历史记录
- 🔐 用户认证系统
- ⚡ 实时响应
- 📱 响应式设计

## 🚀 快速开始

### 前置要求
- Node.js >= 16
- npm 或 yarn
- MongoDB 数据库

### 安装

```bash
# 克隆仓库
git clone https://github.com/haozinbyyds/ai-website.git
cd ai-website

# 安装依赖
npm install
cd client && npm install && cd ..
cd server && npm install && cd ..
```

### 配置环境变量

**根目录 `.env`**
```
REACT_APP_API_URL=http://localhost:5000
```

**server 目录 `.env`**
```
MONGODB_URI=mongodb://localhost:27017/ai-website
JWT_SECRET=your_secret_key_here
OPENAI_API_KEY=your_openai_key_here
PORT=5000
```

### 运行项目

```bash
# 开发模式（根目录）
npm run dev

# 或分别运行前后端
# 终端1：
cd client && npm start

# 终端2：
cd server && npm run dev
```

## 📁 项目结构

```
ai-website/
├── client/                 # React 前端
│   ├── src/
│   │   ├── components/    # 组件
│   │   ├── pages/         # 页面
│   │   ├── styles/        # 样式
│   │   ├── utils/         # 工具函数
│   │   ├── App.tsx        # 主应用
│   │   └── index.tsx      # 入口文件
│   ├── public/            # 静态文件
│   └── package.json
├── server/                 # Express 后端
│   ├── routes/            # API 路由
│   ├── controllers/        # 控制器
│   ├── models/            # 数据模型
│   ├── middleware/        # 中间件
│   ├── server.ts          # 服务器入口
│   └── package.json
├── docs/                   # 文档
└── package.json            # 根配置
```

## 🔧 技术栈

### 前端
- React 18 + TypeScript
- Tailwind CSS
- Axios
- React Router v6

### 后端
- Node.js + Express
- MongoDB + Mongoose
- JWT 认证
- OpenAI API

## 📖 API 文档

### 认证相关
- `POST /api/auth/register` - 用户注册
- `POST /api/auth/login` - 用户登录

### 聊天相关
- `POST /api/chat/message` - 发送消息
- `GET /api/chat/history` - 获取对话历史
- `DELETE /api/chat/:id` - 删除对话

## 🤝 贡献

欢迎提交 Pull Request！

## 📝 许可证

MIT License
