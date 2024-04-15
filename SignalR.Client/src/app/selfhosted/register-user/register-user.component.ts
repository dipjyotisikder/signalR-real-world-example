import { Component, OnInit } from '@angular/core';
import {
  AbstractControl,
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';
import { SelfHostedService } from '../selfhosted.services';
import { User } from 'src/app/models/User';

@Component({
  selector: 'app-register-user',
  templateUrl: './register-user.component.html',
  styleUrl: './register-user.component.css',
})
export class RegisterUserComponent implements OnInit {
  userForm: FormGroup;

  constructor(
    private formBuilder: FormBuilder,
    private selfHostedService: SelfHostedService
  ) {
    this.userForm = this.formBuilder.group({
      fullName: new FormControl('', Validators.required),
      photoUrl: new FormControl(this.selfHostedService.getRandomAvatarUrl()),
    });

    // const fullNameControl = this.userForm.get('fullName');
    // if (fullNameControl) {
    //   fullNameControl.valueChanges.subscribe((value) => {
    //     console.log('Full Name value changed:', value);
    //   });
    // }
  }

  onSubmit() {
    console.log(this.userForm.value);
    console.log('this.userForm', this.userForm.valid);
    console.log('this.userForm.fullName', this.userForm.get('fullName'));

    if (!this.userForm.valid) {
      return;
    }

    this.selfHostedService
      .createUser(this.userForm.value)
      .subscribe((success) => {
        this.userForm.reset();
      });
  }

  ngOnInit(): void {}
}
