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

@Component({
  selector: 'app-message-box',
  templateUrl: './message-box.component.html',
  styleUrls: ['./message-box.component.css'],
})
export class MessageBoxComponent implements OnInit {
  conversationAudience: ConversationAudienceModel | null = null;
  messageList: MessageModel[] = [];
  messageForm: FormGroup;

  constructor(
    private formBuilder: FormBuilder,
    private route: ActivatedRoute,
    private service: SelfHostedService,
    private hubService: HubService
  ) {
    this.messageForm = this.formBuilder.group({
      conversationId: new FormControl(),
      text: new FormControl('', Validators.required),
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
      console.log('create message result: ', success);

      this.messageForm.controls['text'].reset();

      this.messageList.push(success);
    });
  }
}
