import { Component, OnInit } from '@angular/core';
import { UserModel } from 'src/app/models/UserModel';
import { SelfHostedService } from '../selfhosted.services';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { MessageModel } from 'src/app/models/MessageModel';

@Component({
  selector: 'app-message-box',
  templateUrl: './message-box.component.html',
  styleUrls: ['./message-box.component.css'],
})
export class MessageBoxComponent implements OnInit {
  userList: UserModel[] = [];
  messageList: MessageModel[] = [];
  messageForm: FormGroup;

  constructor(
    private formBuilder: FormBuilder,
    private route: ActivatedRoute,
    private service: SelfHostedService
  ) {
    this.messageForm = this.formBuilder.group({
      conversationId: new FormControl(),
      text: new FormControl('', Validators.required),
    });
  }

  ngOnInit() {
    this.service.getUsers().subscribe((success) => {
      this.userList.push(...success);
    });

    this.route.params.subscribe((routeData) => {
      // console.log('routeData', routeData);
      this.messageForm.controls['conversationId'].setValue(+routeData['id']);
      this.service.getMessages(routeData['id']).subscribe((success) => {
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
