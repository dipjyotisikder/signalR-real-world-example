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

@Component({
  selector: 'app-message-box',
  templateUrl: './message-box.component.html',
  styleUrls: ['./message-box.component.css'],
})
export class MessageBoxComponent implements OnInit {
  conversationAudience: ConversationAudienceModel | null = null;
  messageList: MessageModel[] = [];
  messageForm: FormGroup;
  currentUserId: number;

  messageListLoaded: boolean = false;

  constructor(
    private formBuilder: FormBuilder,
    private route: ActivatedRoute,
    private timeAgoPipe: TimeAgoPipe,
    private service: SelfHostedService,
    private hubService: HubService,
    authService: AuthService
  ) {
    this.currentUserId = authService.currentUserId();

    this.messageForm = this.formBuilder.group({
      conversationId: new FormControl(),
      text: new FormControl('', Validators.required),
    });

    this.hubService.listenMessageIsCreatedEvent().subscribe((success) => {
      success && this.messageList.push(success);
    });

    this.hubService.listenUserIsJoinedEvent().subscribe((success) => {
      if (
        success &&
        this.conversationAudience &&
        !this.conversationAudience.audienceUsers.some((x) => x.id == success.id)
      ) {
        this.conversationAudience.audienceUsers.push(success);
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

  onSubmit() {
    // console.log('form', this.messageForm.value);
    if (this.messageForm.invalid) {
      this.messageForm.controls['text'].markAsTouched();
      return;
    }

    this.service.createMessage(this.messageForm.value).subscribe((success) => {
      this.messageForm.controls['text'].reset();
    });
  }

  timeTransform(dateTime: string): string {
    return this.timeAgoPipe.transform(dateTime);
  }
}
