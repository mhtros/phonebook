import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { PageEvent } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { first } from 'rxjs/operators';
import { PaginationParameters } from 'src/app/classes/PaginationParameters';
import { LoaderService } from '../../services/loader.service';
import { PhonebookService } from '../../services/phonebook.service';
import { ManagePhonebookComponent } from '../sub/manage-phonebook/manage-phonebook.component';
import { PaginatedResult } from './../../classes/PaginationParameters';
import { FilterProperty, SearchFilter } from './../../classes/SearchFilters';
import { GetPhonebookDto } from './../../dtos/GetPhonebookDto';

@Component({
  selector: 'app-main',
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.css'],
})
export class MainComponent implements OnInit {
  displayedColumns: string[] = [
    'name',
    'surname',
    'homePhoneNumber',
    'cellPhoneNumber',
    'postCode',
    'district',
    'options',
  ];

  showHideLabel = 'hide';
  isFiltersVisible = true;
  hasData = false;
  filters = new SearchFilter();

  @ViewChild('manageSelect') selectedOption: any;

  orderbyDirection: string;
  orderBy: string;

  pageIndex: number;
  pageSize: number;
  length: number;
  dataSource: MatTableDataSource<GetPhonebookDto>;

  constructor(
    private phonebookService: PhonebookService,
    private matDialog: MatDialog,
    public loader: LoaderService
  ) {
    this.filters.filterProperties = new Array<FilterProperty>();
  }

  public ngOnInit(): void {
    this.getPhonebooksInternal(1, 10);
  }

  public getPhonebooks(event: PageEvent) {
    this.getPhonebooksInternal(event.pageIndex + 1, event.pageSize);
  }

  public addFilter(): void {
    this.filters.filterProperties.push(new FilterProperty());
  }

  public deleteAllFilters(): void {
    this.filters.filterProperties = new Array<FilterProperty>();
  }

  public deleteFilter(index: number): void {
    var elementToDelete = this.filters.filterProperties[index];
    this.filters.filterProperties = this.filters.filterProperties.filter(
      (item) => item != elementToDelete
    );
  }

  public openManageWindow(action: string, phonebook?: GetPhonebookDto): void {
    var data = !!phonebook
      ? { action, phonebook }
      : { action, phonebook: null };

    this.matDialog.open(ManagePhonebookComponent, {
      data,
      disableClose: true,
    });
  }

  public searchWithFilters(): void {
    this.filters.orderBy = '';
    this.filters.orderByDirection = '';

    if (!!this.orderBy) this.filters.orderBy = this.orderBy;
    if (!!this.orderbyDirection)
      this.filters.orderByDirection = this.orderbyDirection;

    this.getPhonebooksInternal(1, 10, this.filters);
  }

  public managePhonebook(value: string, phonebook: GetPhonebookDto): void {
    if (value === 'edit') {
      this.openManageWindow('edit', phonebook);
    } else if (value === 'delete') {
      const msg = `Είστε σίγουροι ότι θέλετε να διαγράψετε την εγγραφή ${phonebook.name}, ${phonebook.surname}?`;
      if (window.confirm(msg)) {
        this.phonebookService
          .deletePhonebook(phonebook.id)
          .pipe(first())
          .subscribe(
            () => {
              window.alert('Η εγγραφή διαγράφηκε επιτυχώς');
              var filterData = this.dataSource.data.filter(
                (p: GetPhonebookDto) => p.id != phonebook.id
              );
              this.dataSource = new MatTableDataSource<GetPhonebookDto>(
                filterData
              );
            },
            (error) => window.alert(`Η εγγραφή δεν διαγράφηκε: ${error}`)
          );
      }
    }
  }

  public showHideFilters(): boolean {
    this.isFiltersVisible = !this.isFiltersVisible;
    this.showHideLabel = this.isFiltersVisible ? 'hide' : 'show';
    return this.isFiltersVisible;
  }

  private getPhonebooksInternal(
    pageNo: number,
    pageSize: number,
    filters?: SearchFilter
  ): void {
    const pagination: PaginationParameters = {
      pageSize: pageSize,
      pageNumber: pageNo,
    };
    this.phonebookService
      .getPhonebooks(pagination, filters)
      .pipe(first())
      .subscribe((res: PaginatedResult<Array<GetPhonebookDto>>) => {
        if (res.result.length == 0) {
          this.hasData = false;
          return;
        }

        this.hasData = true;
        this.dataSource = new MatTableDataSource<GetPhonebookDto>(res.result);
        this.pageIndex = res.pagination.currentPage - 1;
        this.pageSize = res.pagination.itemsPerPage;
        this.length = res.pagination.totalItems;
      });
  }
}
