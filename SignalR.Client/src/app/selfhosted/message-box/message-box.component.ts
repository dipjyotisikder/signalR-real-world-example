import { Component, OnInit } from '@angular/core';
import { User } from 'src/app/models/User';
import { SelfHostedService } from '../selfhosted.services';

@Component({
  selector: 'app-message-box',
  templateUrl: './message-box.component.html',
  styleUrls: ['./message-box.component.css'],
})
export class MessageBoxComponent implements OnInit {
  userList: User[] = [];

  constructor(public service: SelfHostedService) {}

  ngOnInit() {
    this.service
      .getUsers()
      .subscribe((success) => console.log(success, 'users list'));
  }
}
