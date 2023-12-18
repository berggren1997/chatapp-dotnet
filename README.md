# Chat-Application api dotnet

###  Tech stack:
> - C#
> - .NET Core Web API
> - Microsoft SQL Server
> - Entity Framework Core + Identity
> - Github actions



The API is consumed by a React Client. 
[Check out the frontend project here](https://github.com/berggren1997/chatapp-react) 

To set the project up, follow these steps:

# Api endpoints

## Auth Endpoints

### Register Request

```js 
POST /api/authentication/register
```
```json
{
  "username": "bob123",
  "email": "bob@example.com",
  "password": "password123",
  "confirmPassword": "password123"
}
```

### Register Response
```json
{
  "success": true,
  "errors": []
}
```

### Login Request

```js
POST /api/authentication/login
```
```json
{
  "username": "bob123",
  "password": "password123"
}
```

### Login Response
```json
{
  "success": true,
  "username": "bob123",
  "errorMessage": ""
}
```

## Conversations Endpoints

### Get Conversations Request
```js
GET /api/conversation
```

### Get Conversations Response
```json
[
  {
    "id": "47ca6185-59a5-4323-b85a-8d88ea164e4d",
    "createdAt": "2023-06-15T12:29:45Z",
    "conversationDetailsDto": {
      "creator": "bob123",
      "creatorId": "47ca6185-59a5-4323-b85a-8d88ea164e4d",
      "recipient": "charlie",
      "recipientId": "47ca7248-59a5-4323-b85a-8d88ea164e56d"
    },
    "lastMessageDetails": {
      "message": "Hello",
      "sender": "bob123",
      "sentAt": "2023-06-15T12:30:45Z"
    }
  }
]
```

### Create Conversation Request
```js
POST /api/conversation
```
```json
{
  "recipient": "testuser"
}
```

### Create Conversation Response

```json
{
  "conversationId": "47ca6185-59a5-4323-b85a-8d88ea164e4d"
}
```
## Messages
Sending messages requires you to be logged in, and goes through the signalR hub:
```js
/messageHub
```
### Get Messages Request
```js
GET /api/message?conversationId=47ca6185-59a5-4323-b85a-8d88ea164e4d&pageNumber=1&pageSize=50
```

### Get Messages Response
```json
{
  "messages": [
    {
      "message": "hello",
      "sender": "bob123",
      "sentAt": "2023-06-15T12:30:45Z"
    },
    {
      "message": "hi",
      "sender": "charlie",
      "sentAt": "2023-06-15T12:30:45Z"
    }
  ],
  "metaData": {
    "currentPage": 1,
    "totalPages": 3,
    "pageSize": 50,
    "totalCount": 150,
    "hasPrevious": false,
    "hasNext": true
  }
}
```
