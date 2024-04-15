import { AbstractControl } from '@angular/forms';

export class User {
  id?: number;
  fullName?: string;
  photoUrl?: string;

  constructor(id?: number, fullName?: string, photoUrl?: string) {
    this.id = id;
    this.fullName = fullName;
    this.photoUrl = photoUrl;
  }
}
