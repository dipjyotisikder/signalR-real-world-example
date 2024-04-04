import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { NavigationComponent } from './navigation/navigation.component';

@NgModule({
  imports: [CommonModule, RouterModule],

  declarations: [NavigationComponent],

  exports: [NavigationComponent],
})
export class SharedModule {}
