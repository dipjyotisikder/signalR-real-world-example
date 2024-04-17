import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppComponent } from './app.component';
import { AppRoutes } from './app.routes';
import { SharedModule } from './shared/shared.module';
import { ServerlessComponent } from './serverless/serverless.component';
import { SelfHostedComponent } from './selfhosted/selfhosted.component';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MessageBoxComponent } from './selfhosted/message-box/message-box.component';
import { RegisterUserComponent } from './selfhosted/register-user/register-user.component';
import { MessageConversationComponent } from './selfhosted/message-conversation/message-conversation.component';
import { RouterModule } from '@angular/router';
import { TokenInterceptor } from './shared/token.interceptor';
import { TimeAgoPipe } from './shared/timeAgo.pipe';

@NgModule({
  imports: [
    CommonModule,
    BrowserModule,
    AppRoutes,
    SharedModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    RouterModule,
  ],

  declarations: [
    AppComponent,
    ServerlessComponent,
    SelfHostedComponent,
    MessageBoxComponent,
    RegisterUserComponent,
    MessageConversationComponent,
  ],

  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: TokenInterceptor,
      multi: true,
    },
    TimeAgoPipe,
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
