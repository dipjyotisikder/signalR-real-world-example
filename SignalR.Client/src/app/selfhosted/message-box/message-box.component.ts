import { Component, OnInit } from '@angular/core';
import { SelfHostedService } from '../selfhosted.services';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { MessageModel } from 'src/app/models/MessageModel';
import { ConversationAudienceModel } from 'src/app/models/ConversationModel';
import { HubService } from '../../shared/hub.services';
import { AuthService } from 'src/app/shared/auth.service';
import { TimeAgoPipe } from 'src/app/shared/timeAgo.pipe';
import { UserModel } from 'src/app/models/UserModel';

@Component({
  selector: 'app-message-box',
  templateUrl: './message-box.component.html',
  styleUrls: ['./message-box.component.css'],
})
export class MessageBoxComponent implements OnInit {
  currentUserId: number;

  conversationAudience: ConversationAudienceModel | null = null;

  messageList: MessageModel[] = [];
  messageListLoaded: boolean = false;

  messageForm: FormGroup;

  typingUsers: UserModel[] = [];
  typingInterval: number = 2000;
  typingTimeout: any = null;

  typingFocused: boolean = false;
  typingFocusedInterval: any = null;

  constructor(
    private formBuilder: FormBuilder,
    private route: ActivatedRoute,
    private timeAgoPipe: TimeAgoPipe,
    private service: SelfHostedService,
    private hubService: HubService,
    authService: AuthService
  ) {
    // INIT CURRENT USER ID
    this.currentUserId = authService.currentUserId();

    // INIT MESSAGE FORM
    this.messageForm = this.formBuilder.group({
      conversationId: new FormControl(),
      text: new FormControl('', Validators.required),
    });

    // INIT LISTENER FOR MESSAGE_IS_CREATED EVENT
    this.hubService.messageIsCreatedEventHandler().subscribe((success) => {
      if (!success) return;
      this.messageList.push(success);
      success.creatorUser && this.removeTyping(success.creatorUser);
    });

    // INIT LISTENER FOR USER_IS_JOINED EVENT
    this.hubService.userIsJoinedEventHandler().subscribe((success) => {
      // console.log('Joined', success);
      if (!success) return;
      if (
        this.conversationAudience && !this.conversationAudience.audienceUsers.some((x) => x.id == success.id)
      ) {
        this.conversationAudience.audienceUsers.push(success);
      }
    });

    // INIT LISTENER FOR USER_IS_TYPING EVENT
    this.hubService.userIsTypingEventHandler().subscribe((success) => {
      if (!success) return;
      if (success.isTyping) {
        this.activateTyping(success);
      } else {
        this.removeTyping(success);
      }
    });
  }

  ngOnInit() {
    this.hubService.startHub();

    this.route.params.subscribe((routeData) => {
      const conversationId = +routeData['id'];

      this.messageForm.controls['conversationId'].setValue(conversationId);

      this.service
        .getConversationAudiences(conversationId)
        .subscribe((success) => {
          this.conversationAudience = success;
        });

      this.service.getMessages(conversationId).subscribe((success) => {
        this.messageList.push(...success);
        this.messageListLoaded = true;
      });
    });
  }

  onSubmit(textInput: HTMLInputElement) {
    if (this.messageForm.invalid) {
      this.messageForm.controls['text'].markAsTouched();
      return;
    }

    this.service.createMessage(this.messageForm.value).subscribe((success) => {
      this.messageForm.controls['text'].reset();
    });

    textInput.blur();
  }

  timeTransform(dateTime: string): string {
    return this.timeAgoPipe.transform(dateTime);
  }

  activateTyping(user: UserModel) {
    if (!this.typingUsers.some((x) => x.id == user.id)) {
      this.typingUsers.push(user);
    }

    this.typingTimeout && clearTimeout(this.typingTimeout);
    this.typingTimeout = setTimeout(() => {
      this.typingUsers = [];
    }, this.typingInterval);
  }

  removeTyping(user: UserModel) {
    this.typingUsers = this.typingUsers.filter((x) => x.id != user.id);
  }

  onBlur(event: any) {
    // console.log('onBlur', event);
    this.typingFocused = false;
    this.typingFocusedInterval && clearInterval(this.typingFocusedInterval);
    this.conversationAudience && this.hubService.triggerUserIsTypingEvent(this.conversationAudience.id, false);
  }

  onFocus(event: any) {
    // console.log('onFocus', event);
    this.typingFocused = true;
    this.typingFocusedInterval && clearInterval(this.typingFocusedInterval);
    this.typingFocusedInterval = setInterval(() => {
      if (this.typingFocused) {
        this.conversationAudience && this.hubService.triggerUserIsTypingEvent(this.conversationAudience.id, true);
      }
    }, 1000);
  }
}
