import {
  AfterViewInit,
  Component,
  Inject,
  OnInit,
  Optional,
} from '@angular/core';
import { FormGroup } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { AddPhonebookDto } from 'src/app/dtos/AddPhonebookDto';
import { EditPhonebookDto } from 'src/app/dtos/EditPhonebookDto';
import { GetPhonebookDto } from 'src/app/dtos/GetPhonebookDto';
import { GetDistrictDto } from './../../../dtos/GetDistrictDto';
import { DistrictService } from './../../../services/district.service';
import { PhonebookService } from './../../../services/phonebook.service';

@Component({
  selector: 'app-manage-phonebook',
  templateUrl: './manage-phonebook.component.html',
  styleUrls: ['./manage-phonebook.component.css'],
})
export class ManagePhonebookComponent implements OnInit, AfterViewInit {
  public name: string;
  public surname: string;
  public email: string;
  public homePhoneNumber: string;
  public cellPhoneNumber: string;
  public districtId: number;

  public title: string;
  public districts: Array<GetDistrictDto>;

  constructor(
    private phonebookService: PhonebookService,
    private districtService: DistrictService,
    public dialogRef: MatDialogRef<ManagePhonebookComponent>,
    @Optional()
    @Inject(MAT_DIALOG_DATA)
    public data: { action: string; phonebook: GetPhonebookDto }
  ) {
    this.title =
      this.data.action === 'edit'
        ? 'Επεξεργασία εγγραφής'
        : 'Δημιουργία εγγραφής';
    this.fillFormProperties();
  }

  ngOnInit(): void {}

  public ngAfterViewInit(): void {
    setTimeout(() => this.getDistricts(), 0);
  }

  private fillFormProperties(): void {
    if (this.data.action === 'edit' || !!this.data.phonebook) {
      this.name = this.data.phonebook.name;
      this.surname = this.data.phonebook.surname;
      this.email = this.data.phonebook.email;
      this.cellPhoneNumber = this.data.phonebook.cellPhoneNumber;
      this.homePhoneNumber = this.data.phonebook.cellPhoneNumber;
      this.districtId = this.data.phonebook.district.id;
    }
  }

  private getDistricts(): void {
    this.districtService
      .getDistricts()
      .subscribe((res: Array<GetDistrictDto>) => (this.districts = res));
  }

  private closeWindow(): void {
    this.dialogRef.close();
  }

  public manage(form: FormGroup): void {
    if (this.data.action === 'edit') {
      let requestData: EditPhonebookDto = {
        id: this.data.phonebook.id,
        name: form.get('Name')?.value,
        surname: form.get('Surname')?.value,
        email: form.get('Email')?.value,
        cellPhoneNumber: form.get('CellPhoneNumber')?.value,
        homePhoneNumber: form.get('HomePhoneNumber')?.value,
        districtId: form.get('DistrictId')?.value,
      };
      this.phonebookService.editPhonebook(requestData).subscribe(
        () => window.alert('Οι αλλαγές αποθηκεύτηκαν επιτυχώς'),
        (error) => window.alert(`Οι αλλαγές δεν αποθηκεύτηκαν : ${error}`)
      );
    } else if (this.data.action === 'add') {
      let requestData: AddPhonebookDto = {
        name: form.get('Name')?.value,
        surname: form.get('Surname')?.value,
        email: form.get('Email')?.value,
        cellPhoneNumber: form.get('CellPhoneNumber')?.value,
        homePhoneNumber: form.get('HomePhoneNumber')?.value,
        districtId: form.get('DistrictId')?.value,
      };
      this.phonebookService.addPhonebook(requestData).subscribe(
        () => window.alert('Η Εγγραφή αποθηκεύτηκε επιτυχώς'),
        (error) => window.alert(`Η Εγγραφή δεν αποθηκεύτηκε: ${error}`)
      );
    }
    this.closeWindow();
  }
}
