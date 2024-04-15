import { Component, OnInit } from '@angular/core';
import {
  AbstractControl,
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';
import { SelfHostedService } from '../selfhosted.services';
import { UserModel } from 'src/app/models/UserModel';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register-user',
  templateUrl: './register-user.component.html',
  styleUrl: './register-user.component.css',
})
export class RegisterUserComponent implements OnInit {
  userForm: FormGroup;

  constructor(
    private formBuilder: FormBuilder,
    private selfHostedService: SelfHostedService,
    private router: Router
  ) {
    this.userForm = this.formBuilder.group({
      fullName: new FormControl('', Validators.required),
      photoUrl: new FormControl(''),
    });

    // const fullNameControl = this.userForm.get('fullName');
    // if (fullNameControl) {
    //   fullNameControl.valueChanges.subscribe((value) => {
    //     console.log('Full Name value changed:', value);
    //   });
    // }
  }

  onSubmit() {
    if (!this.userForm.valid) {
      // this.userForm.controls['fullName'].markAsTouched();
      this.userForm.markAllAsTouched();
      return;
    }

    this.userForm.controls['photoUrl'].setValue(
      this.selfHostedService.getRandomAvatarUrl()
    );

    this.selfHostedService
      .createUser(this.userForm.value)
      .subscribe((success) => {
        this.userForm.reset();
        this.router.navigate(['/selfhosted/messaging']);
      });
  }

  ngOnInit(): void {}
}
