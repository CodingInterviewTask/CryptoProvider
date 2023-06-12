import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ChangeSymbolRequest, CryptoData } from './app.models';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CryptoService {

  constructor(
    private http: HttpClient
  ) { }

  changeSymbol(request: ChangeSymbolRequest): Observable<CryptoData> {
    return this.http.post<CryptoData>(`http://localhost:5001/api/Crypto/ChangeSymbol`, request);
  }
}
