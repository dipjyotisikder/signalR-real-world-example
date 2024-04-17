import { Component, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';
import { SelfHostedService } from '../selfhosted.services';
import { ConversationModel } from 'src/app/models/ConversationModel';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/shared/auth.service';

@Component({
  selector: 'app-message-conversation',
  templateUrl: './message-conversation.component.html',
  styleUrl: './message-conversation.component.css',
})
export class MessageConversationComponent implements OnInit {
  conversationForm: FormGroup;
  currentUserId: number;

  myConversationList: ConversationModel[] = [];
  otherConversationList: ConversationModel[] = [];

  constructor(
    private formBuilder: FormBuilder,
    private router: Router,
    private selfHostedService: SelfHostedService,
    authService: AuthService
  ) {
    this.currentUserId = authService.currentUserId();

    this.conversationForm = this.formBuilder.group({
      title: new FormControl('', Validators.required),
      creatorUserId: new FormControl(0),
    });

    console.log(this.currentUserId);
  }

  ngOnInit(): void {
    this.selfHostedService.getConversations().subscribe((success) => {
      this.myConversationList.push(
        ...success.filter((x) => x.creatorUser?.id == this.currentUserId)
      );
      this.otherConversationList.push(
        ...success.filter((x) => x.creatorUser?.id != this.currentUserId)
      );
    });
  }

  onSubmit(event: any, inputElement: HTMLInputElement) {
    inputElement.blur();

    if (this.conversationForm.invalid) {
      this.conversationForm.controls['title'].markAsTouched();
      return;
    }

    this.conversationForm.controls['creatorUserId'].setValue(0);
    this.selfHostedService
      .createConversation(this.conversationForm.value)
      .subscribe((success) => {
        console.log('conversation create result', success);
        this.myConversationList.push(success);
        this.conversationForm.reset();
      });
  }

  navigateConversation(conversationId: number) {
    console.log('conversationId', conversationId);
    this.router.navigate([`/selfhosted/messaging/${conversationId}`]);
  }
}
