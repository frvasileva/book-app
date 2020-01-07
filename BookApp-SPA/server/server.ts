import 'zone.js/dist/zone-node';
import 'reflect-metadata';
import { renderModuleFactory } from '@angular/platform-server';
import express from 'express';
import { readFileSync } from 'fs';
import { enableProdMode } from '@angular/core';
import proxy from 'http-proxy-middleware';

const { AppServerModuleNgFactory } = require('../dist/chetime-app-server/main');
const indexHtml = readFileSync(__dirname + '/../dist/chetime-app/index.html', 'utf-8').toString();
const app = express();

enableProdMode();

// proxy api requests to the URL where the .NET server lives
app.use('/api', proxy({
  target: 'http://192.168.1.106:5000',
  changeOrigin: true,
  logLevel: 'debug'
}));

// serve static files from the dist folder
app.get('*.*', express.static(__dirname + '/../dist/chetime-app'));

// all other GET requests are to be served by the universal app
app.get('*', (req, res) => {
  renderModuleFactory(AppServerModuleNgFactory, {
    document: indexHtml,
    url: req.url
  })
    .then(html => {
      res.status(200).send(html);
    })
    .catch(err => {
      console.log(err);
      res.sendStatus(500);
    });
});

app.listen(9000, () => {
  console.log(`Angular Universal Node Express server listening on http://localhost:9000`);
});
