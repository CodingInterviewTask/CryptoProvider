import { Component, OnInit } from '@angular/core';
import { AuthService } from './auth.service';
import * as signalR from '@microsoft/signalr';
import { ChangeSymbolRequest, CryptoData } from './app.models';
import { CryptoService } from './crypto.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  
  private hubConnection!: signalR.HubConnection;
  
  cryptoData: CryptoData | undefined;
  selectedSymbol = 'BTCUSDT';
  symbols: string[] = [];

  constructor(
      private authService: AuthService,
      private cryptoService: CryptoService,
    ) {}

  ngOnInit(): void {
    this.symbols = [
      'BTCUSDT',
      'ETHUSDT',
      'DOGEUSDT',
      'ETHBTC',
      'DOGEBTC',
    ];
  }

  onLogin() {
    this.authService.login();
  }

  onStart() {
    this.startConnection();
  }

  onChangeSelectedSymbol() {
    let request : ChangeSymbolRequest = {
      symbol: this.selectedSymbol
    }

    this.cryptoService.changeSymbol(request).subscribe(response => this.cryptoData = response);
  }

  private startConnection() {
    this.hubConnection = new signalR.HubConnectionBuilder()
        .withUrl(`http://localhost:5001/CryptoHub`)
        .withAutomaticReconnect()
        .build();
        
    this.hubConnection
        .start()
        .then(() => console.log('SignalR connection started'))
        .catch(error => console.log('SignalR error: ' + error));
    
    this.onCryptoDataResponse();
  }

  private onCryptoDataResponse = () => {
    this.hubConnection.on('CryptoDataResponse', (response: CryptoData) => {
      this.cryptoData = response;
    });
  }
}
