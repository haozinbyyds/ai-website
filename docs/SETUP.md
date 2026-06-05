# AI Website 安装指南

## 环境要求

- Node.js >= 16.0.0
- npm >= 8.0.0 或 yarn >= 1.22.0
- MongoDB >= 4.4
- Git

## 安装步骤

### 1. 克隆仓库

```bash
git clone https://github.com/haozinbyyds/ai-website.git
cd ai-website
```

### 2. 安装依赖

```bash
# 安装根目录依赖
npm install

# 安装前端依赖
cd client
npm install
cd ..

# 安装后端依赖
cd server
npm install
cd ..
```

### 3. 配置环境变量

在项目根目录创建 `.env` 文件：

```env
REACT_APP_API_URL=http://localhost:5000
```

在 `server` 目录创建 `.env` 文件：

```env
PORT=5000
MONGODB_URI=mongodb://localhost:27017/ai-website
JWT_SECRET=your_super_secret_jwt_key_here
OPENAI_API_KEY=your_openai_api_key_here
NODE_ENV=development
```

### 4. 启动 MongoDB

**使用本地 MongoDB：**

```bash
mongod
```

**或使用 Docker：**

```bash
docker run -d -p 27017:27017 --name mongodb mongo:latest
```

### 5. 启动项目

**方式一：同时启动前后端（在根目录）**

```bash
npm run dev
```

**方式二：分别启动（推荐用于开发）**

终端1 - 启动后端：
```bash
cd server
npm run dev
```

终端2 - 启动前端：
```bash
cd client
npm start
```

### 6. 访问应用

- 前端：http://localhost:3000
- 后端 API：http://localhost:5000

## 故障排除

### 端口被占用

```bash
# 查找占用端口3000的进程（macOS/Linux）
lsof -i :3000

# 杀死进程
kill -9 <PID>
```

### MongoDB 连接失败

确保 MongoDB 服务正在运行，并检查 `MONGODB_URI` 配置。

### 依赖安装失败

```bash
# 清除缓存
npm cache clean --force

# 重新安装
rm -rf node_modules package-lock.json
npm install
```

## 生产环境部署

### 构建

```bash
npm run build
```

### 使用 PM2 启动

```bash
npm install -g pm2
pm2 start server/dist/server.js --name "ai-website"
```
