import { RouterModule, Routes } from '@angular/router';
import { SelfHostedComponent } from './selfhosted/selfhosted.component';
import { ServerlessComponent } from './serverless/serverless.component';

export const routes: Routes = [
  {
    path: '',
    redirectTo: '/selfhosted',
    pathMatch: 'full',
  },
  {
    path: 'serverless',
    component: ServerlessComponent,
  },
  {
    path: 'selfhosted',
    component: SelfHostedComponent,
  },
];

export const AppRoutes = RouterModule.forRoot(routes);
