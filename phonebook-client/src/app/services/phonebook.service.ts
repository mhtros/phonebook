import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { GetPhonebookDto } from 'src/app/dtos/GetPhonebookDto';
import { environment } from 'src/environments/environment';
import {
  PaginatedResult,
  PaginationParameters,
} from '../classes/PaginationParameters';
import { SearchFilter } from '../classes/SearchFilters';
import { AddPhonebookDto } from '../dtos/AddPhonebookDto';
import { EditPhonebookDto } from '../dtos/EditPhonebookDto';

@Injectable({
  providedIn: 'root',
})
export class PhonebookService {
  private readonly controllerUrl = `${environment.baseUrl}/phonebook`;

  constructor(private http: HttpClient) {}

  public getSinglePhonebook(id: number): Observable<GetPhonebookDto> {
    return this.http.get<GetPhonebookDto>(`${this.controllerUrl}/${id}`);
  }

  public deletePhonebook(id: number): Observable<void> {
    return this.http.delete<void>(`${this.controllerUrl}/${id}`);
  }

  public editPhonebook(dto: EditPhonebookDto): Observable<void> {
    return this.http.put<void>(`${this.controllerUrl}/edit`, dto);
  }

  public addPhonebook(dto: AddPhonebookDto): Observable<void> {
    return this.http.post<void>(`${this.controllerUrl}/add`, dto);
  }

  public getPhonebooks(
    pagination: PaginationParameters,
    filters?: SearchFilter
  ): Observable<PaginatedResult<Array<GetPhonebookDto>>> {
    let params = new HttpParams();

    const paginatedResult = new PaginatedResult<Array<GetPhonebookDto>>();

    params = params.append('pageSize', pagination.pageSize);
    params = params.append('pageNumber', pagination.pageNumber);

    if (!!filters) {
      if (!!filters.orderBy) params = params.append('orderby', filters.orderBy);

      if (!!filters.orderByDirection)
        params = params.append('orderByDirection', filters.orderByDirection);

      if (!!filters.filterProperties)
        for (let i = 0; i < filters.filterProperties.length; i++) {
          params = params.append(
            `FilterProperties[${i}].propertyName`,
            filters.filterProperties[i].propertyName
          );
          params = params.append(
            `FilterProperties[${i}].propertyValue`,
            filters.filterProperties[i].propertyValue
          );
        }
    }

    return this.http
      .get<Array<GetPhonebookDto>>(this.controllerUrl, {
        observe: 'response',
        params: params,
      })
      .pipe(
        map((response) => {
          paginatedResult.result =
            response.body ?? new Array<GetPhonebookDto>();

          if (response.headers.get('Pagination') != null) {
            paginatedResult.pagination = JSON.parse(
              response.headers.get('Pagination') ?? ''
            );
          }
          return paginatedResult;
        })
      );
  }
}
