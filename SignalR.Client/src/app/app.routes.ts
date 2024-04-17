import { RouterModule, Routes } from '@angular/router';
import { SelfHostedComponent } from './selfhosted/selfhosted.component';
import { ServerlessComponent } from './serverless/serverless.component';
import { MessageBoxComponent } from './selfhosted/message-box/message-box.component';
import { MessageConversationComponent } from './selfhosted/message-conversation/message-conversation.component';
import { AuthGuard } from './shared/auth.guard';

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
  {
    path: 'selfhosted/messaging',
    component: MessageConversationComponent,
    canActivate: [AuthGuard],
  },
  {
    path: 'selfhosted/messaging/:id',
    component: MessageBoxComponent,
    canActivate: [AuthGuard],
  },
];

export const AppRoutes = RouterModule.forRoot(routes);
