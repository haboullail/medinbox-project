// notify-server.js
import express, { Request, Response } from 'express';
import bodyParser from 'body-parser';
import { WebSocketServer } from 'ws';
import http from 'http';

const app = express();
app.use(bodyParser.json());

const server = http.createServer(app);
const wss = new WebSocketServer({ server });

wss.on('connection', (ws) => {
  console.log('WebSocket client connected');
});

// Endpoint HTTP pour recevoir les requêtes de .NET
app.post('/notify', (req: Request, res: Response) => {
  const data = req.body;

  // Diffuser à tous les clients WebSocket
  wss.clients.forEach(client => {
    if (client.readyState === client.OPEN) {
      client.send(JSON.stringify(data));
    }
  });

  res.status(200).send('Notified');
});

server.listen(3002, () => {
  console.log('Server started on http://localhost:3002 and ws://localhost:3002');
});
