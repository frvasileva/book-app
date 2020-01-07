import 'zone.js/dist/zone-node';
import 'reflect-metadata';
import express from 'express';
import proxy from 'http-proxy-middleware';
import { enableProdMode } from '@angular/core';
import { ngExpressEngine } from '@nguniversal/express-engine';
import { provideModuleMap } from '@nguniversal/module-map-ngfactory-loader';
import { AppServerModuleNgFactory, LAZY_MODULE_MAP } from '../dist/chetime-app-server/main';

enableProdMode();

const app = express();
app.engine('html', ngExpressEngine({
  bootstrap: AppServerModuleNgFactory,
  providers: [provideModuleMap(LAZY_MODULE_MAP)]
}));
app.set('view engine', 'html');

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
  res.render('../dist/chetime-app/index', {
    req,
    res
  });
});

app.listen(9000, () => {
  console.log(`Angular Universal Node Express server listening on http://localhost:9000`);
});
