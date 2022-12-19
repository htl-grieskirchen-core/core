import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-password-input',
  templateUrl: './password-input.component.html',
  styleUrls: ['./password-input.component.css']
})
export class PasswordInputComponent implements OnInit {
  valid : boolean = true
  constructor() { }

  ngOnInit(): void {
    console.log()
  }

  onClick(b: boolean) {
    console.log(b)
  }
}
