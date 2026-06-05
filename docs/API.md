# AI Website API 文档

## 基础信息

- **基础 URL**: `http://localhost:5000/api`
- **认证**: JWT Token（除了认证端点）
- **响应格式**: JSON

## 错误响应

所有错误响应都遵循以下格式：

```json
{
  "error": "错误消息",
  "code": "ERROR_CODE"
}
```

## 端点

### 认证

#### 注册用户

```
POST /auth/register
```

**请求体:**
```json
{
  "email": "user@example.com",
  "password": "password123",
  "name": "用户名"
}
```

**响应:**
```json
{
  "token": "jwt_token_here",
  "user": {
    "id": "user_id",
    "email": "user@example.com",
    "name": "用户名"
  }
}
```

#### 登录

```
POST /auth/login
```

**请求体:**
```json
{
  "email": "user@example.com",
  "password": "password123"
}
```

**响应:**
```json
{
  "token": "jwt_token_here",
  "user": {
    "id": "user_id",
    "email": "user@example.com",
    "name": "用户名"
  }
}
```

### 聊天

#### 发送消息

```
POST /chat/message
Authorization: Bearer <token>
```

**请求体:**
```json
{
  "message": "你好，今天天气怎么样？"
}
```

**响应:**
```json
{
  "id": "message_id",
  "message": "你好，今天天气怎么样？",
  "reply": "AI的回复内容",
  "timestamp": "2024-01-01T12:00:00Z"
}
```

#### 获取对话历史

```
GET /chat/history
Authorization: Bearer <token>
```

**查询参数:**
- `limit` (可选): 返回的消息数量，默认为 50
- `offset` (可选): 分页偏移量，默认为 0

**响应:**
```json
{
  "messages": [
    {
      "id": "message_id",
      "message": "用户消息",
      "reply": "AI回复",
      "timestamp": "2024-01-01T12:00:00Z"
    }
  ],
  "total": 100,
  "limit": 50,
  "offset": 0
}
```

#### 删除对话

```
DELETE /chat/:id
Authorization: Bearer <token>
```

**响应:**
```json
{
  "success": true,
  "message": "对话已删除"
}
```

## 示例

### 使用 cURL

```bash
# 登录
curl -X POST http://localhost:5000/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"email":"user@example.com","password":"password123"}'

# 发送消息
curl -X POST http://localhost:5000/api/chat/message \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer YOUR_TOKEN" \
  -d '{"message":"你好"}'
```

### 使用 JavaScript/Fetch

```javascript
const token = localStorage.getItem('token');

// 发送消息
fetch('http://localhost:5000/api/chat/message', {
  method: 'POST',
  headers: {
    'Content-Type': 'application/json',
    'Authorization': `Bearer ${token}`
  },
  body: JSON.stringify({ message: '你好' })
})
.then(res => res.json())
.then(data => console.log(data));
```

## 状态码

- `200`: 成功
- `201`: 创建成功
- `400`: 请求错误
- `401`: 未认证
- `403`: 禁止访问
- `404`: 未找到
- `500`: 服务器错误
