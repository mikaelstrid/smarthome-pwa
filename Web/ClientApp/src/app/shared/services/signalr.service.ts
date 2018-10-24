import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { HubConnection } from '@aspnet/signalr';
import * as signalR from '@aspnet/signalr';
import { Observable } from 'rxjs';
import { Subject } from 'rxjs';
import { SignalRConnectionInfo } from '../models/signalr-connection-info.model';

@Injectable({
   providedIn: 'root',
})
export class SignalRService {
   private readonly _http: HttpClient;
   private readonly _baseUrl: string =
      // 'http://localhost:7071/api/';
      'https://smarthomereceiversappservice.azurewebsites.net/api/';
   private hubConnection: HubConnection;
   messages: Subject<string> = new Subject();

   constructor(http: HttpClient) {
      this._http = http;
   }

   private getConnectionInfo(): Observable<SignalRConnectionInfo> {
      const requestUrl = `${this._baseUrl}negotiate`;
      return this._http.get<SignalRConnectionInfo>(requestUrl);
   }

   init() {
      this.getConnectionInfo().subscribe(info => {
         console.log(info);

         const options = {
            accessTokenFactory: () => info.accessKey,
         };

         this.hubConnection = new signalR.HubConnectionBuilder()
            .withUrl(this._baseUrl)
            .configureLogging(signalR.LogLevel.Information)
            .build();

         this.hubConnection.start().catch(err => console.error(err.toString()));

         this.hubConnection.on('update-temperature-humidity', (data: any) => {
            console.log(data);
            // this.messages.next(data);
         });
      });
   }

   //   send(message: string): Observable<void> {
   //       const requestUrl = `${this._baseUrl}message`;
   //       return this._http.post(requestUrl, message).pipe(map((result: any) => { }));
   //   }
}
