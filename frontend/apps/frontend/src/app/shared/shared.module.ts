import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {EmailInputComponent} from "./email-input/email-input.component";
import {PasswordInputComponent} from "./password-input/password-input.component";
import {FormsModule} from "@angular/forms";
import {RouterLink} from "@angular/router";



@NgModule({
  declarations: [
    EmailInputComponent,
    PasswordInputComponent
  ],
  imports: [
    CommonModule,
    RouterLink,
    FormsModule
  ],
  exports: [
    EmailInputComponent,
    PasswordInputComponent
  ],

})
export class SharedModule { }
