import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { AccessTokenModel } from './app.models';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  
  constructor(
    private http: HttpClient,
    private jwtHelper: JwtHelperService,
  ) { }

  login() : void {
    this.http.post<AccessTokenModel>(`http://localhost:5000/api/Auth/LogIn`, null).subscribe(
      token => sessionStorage.setItem('access_token', token.accessToken)
    );
  }
}
