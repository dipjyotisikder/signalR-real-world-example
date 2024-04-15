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

@Component({
  selector: 'app-message-conversation',
  templateUrl: './message-conversation.component.html',
  styleUrl: './message-conversation.component.css',
})
export class MessageConversationComponent implements OnInit {
  conversationForm: FormGroup;

  conversationList: ConversationModel[] = [];

  constructor(
    private formBuilder: FormBuilder,
    private router: Router,
    private selfHostedService: SelfHostedService
  ) {
    this.conversationForm = this.formBuilder.group({
      title: new FormControl('', Validators.required),
      creatorUserId: new FormControl(0),
    });
  }

  ngOnInit(): void {
    this.selfHostedService.getConversations().subscribe((success) => {
      this.conversationList.push(...success);

      console.log('conversations', success);
    });
  }

  onSubmit(event: any, inputElement: HTMLInputElement) {
    inputElement.blur();

    if (this.conversationForm.invalid) {
      this.conversationForm.controls['title'].markAsTouched();
      return;
    }

    // console.log('form', this.conversationForm.value);
    this.conversationForm.controls['creatorUserId'].setValue(0);
    this.selfHostedService
      .createConversation(this.conversationForm.value)
      .subscribe((success) => {
        console.log('conversation create result', success);
        this.conversationList.push(success);
        this.conversationForm.reset();
      });
  }

  navigateConversation(conversationId: number) {
    console.log('conversationId', conversationId);
    this.router.navigate([`/selfhosted/messaging/${conversationId}`]);
  }
}
