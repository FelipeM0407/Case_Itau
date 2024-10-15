import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Fundo } from './Fundo';
import { Observable } from 'rxjs';

const httpOptions ={
  headers: new HttpHeaders({
    'Content-Type' : 'application/json'
  })
}

@Injectable({
  providedIn: 'root'
})
export class FundosService {

  url = 'https://localhost:5001/api/Fundo';


  constructor(private http: HttpClient) { }

  GetAll() : Observable<Fundo[]>{
    return this.http.get<Fundo[]>(this.url);
  }

  GetByCodigo(FundoCodigo: string) : Observable<Fundo>{
    const apiUrl = `${this.url}/${FundoCodigo}`;

    return this.http.get<Fundo>(apiUrl);
  };

  SaveFundo(Fundo: Fundo) : Observable<any>{
    return this.http.post<Fundo>(this.url, Fundo, httpOptions);
  };

  UpdateFundo(Fundo: Fundo) : Observable<any> {
    const apiUrl = `${this.url}/${Fundo.codigo}`;

    return this.http.put<Fundo>(apiUrl, Fundo, httpOptions);
  };

  AddFundo(fundoCodigo: string, fundoPatrimonio: number) : Observable<any> {
    const apiUrl = `${this.url}/${fundoCodigo}/patrimonio`;

    return this.http.put<Fundo>(apiUrl, fundoPatrimonio, httpOptions);
  };

  DeleteFundo(FundoCodigo: string) : Observable<any>{
    const apiUrl = `${this.url}/${FundoCodigo}`;
    return this.http.delete<number>(apiUrl, httpOptions);
  }
}
