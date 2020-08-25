import { Component, OnInit } from '@angular/core';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {NoticeService} from '../../services/notice.service';
import {SpecialNotice} from '../../../../shared/models/SpecialNotice';

@Component({
  selector: 'app-notice-form',
  templateUrl: './notice-form.component.html',
  styleUrls: ['./notice-form.component.scss']
})
export class NoticeFormComponent implements OnInit {

  public noticeForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private noticeService: NoticeService,
  ) { }

  public hasError(formControl: string): boolean {
    return this.noticeForm.get(formControl).hasError('required') && this.noticeForm.get(formControl).touched;
  }

  ngOnInit(): void {
    this.createForm();

    this.noticeService.getSpecialNotice().subscribe(
      resp => {
        this.noticeForm.get('notice').setValue(resp.notice);
        this.noticeForm.get('status').setValue(resp.status + '');
      }, err => {
        if (err.error.status !== 404) {
          console.error(err);
        }
      }
    );
  }

  private createForm(): void {
    this.noticeForm = this.fb.group({
      notice: ['', Validators.required],
      status: ['1', Validators.required],
    });
  }

  public submit(): void {
    if (!this.noticeForm.valid) {
      return;
    }

    const val = this.noticeForm.value;
    const notice: SpecialNotice = {
      notice: val.notice,
      status: parseInt(val.status, 10)
    };

    this.noticeService.setSpecialNotice(notice).subscribe(
      () => {
        alert('Updated notice');
      }, err => {
        console.error(err);
      }
    );

  }

  public remove(): void {
    this.noticeService.removeSpecialNotice().subscribe(
      () => {
        alert('Removed notice');
        this.noticeForm.get('notice').setValue('');
        this.noticeForm.get('status').setValue('1');
      }, err => {
        console.error(err);
      }
    );
  }
}
