import { Routes } from '@angular/router';

import { ProfileComponent } from './profile/profile.component';
import { AlertComponent } from './alerts/alerts.component';

export const routes: Routes = [
  { path: '', component: AlertComponent },        // default route loads AlertComponent
  { path: 'profile', component: ProfileComponent },
];
