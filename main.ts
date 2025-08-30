import { bootstrapApplication } from '@angular/platform-browser';
import { appConfig } from './app/app.config';
import { AppComponent } from './app/app';
console.log('[main] bootstrapping...');
bootstrapApplication(AppComponent, appConfig)
 .then(() => console.log('[main] bootstrapped'))
 .catch(err => console.error(err));