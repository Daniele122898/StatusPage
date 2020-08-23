import { Component, OnInit } from '@angular/core';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {LoginRequest} from '../../../../shared/models/Login';
import {AuthService} from '../../../../shared/Services/auth.service';
import {Router} from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  public loginForm: FormGroup;
  public hidePassword = true;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router,
  ) { }

  ngOnInit(): void {
    if (this.authService.loggedIn()) {
      this.router.navigate(['/dashboard']);
    }
    this.createForm();
  }

  public hasError(formControl: string): boolean {
    return this.loginForm.get(formControl).hasError('required') && this.loginForm.get(formControl).touched;
  }

  private createForm(): void {
    this.loginForm = this.fb.group({
      username: ['', Validators.required],
      password: ['', Validators.required]
    });
  }

  public login(): void {
    if (!this.loginForm.valid) {
      return;
    }

    const loginRequest: LoginRequest = this.loginForm.value;
    this.authService.login(loginRequest.username, loginRequest.password).subscribe(
      resp => {
        this.router.navigate(['/dashboard']);
      }, err => {
        alert(err.error.status + ' ' + err.error.title);
        console.error(err);
      }
    );
  }
}
