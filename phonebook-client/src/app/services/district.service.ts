import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { GetDistrictDto } from '../dtos/GetDistrictDto';

@Injectable({
  providedIn: 'root',
})
export class DistrictService {
  private readonly controllerUrl = `${environment.baseUrl}/district`;

  constructor(private http: HttpClient) {}

  public getDistricts(): Observable<Array<GetDistrictDto>> {
    return this.http.get<Array<GetDistrictDto>>(this.controllerUrl);
  }
}
