import { Component, OnInit } from '@angular/core';
import {FormBuilder, FormGroup} from '@angular/forms';

@Component({
  selector: 'app-status-config-form',
  templateUrl: './status-config-form.component.html',
  styleUrls: ['./status-config-form.component.scss']
})
export class StatusConfigFormComponent implements OnInit {

  public statusConfigForm: FormGroup;

  constructor(
    private fb: FormBuilder,
  ) { }

  public hasError(formControl: string): boolean {
    return this.statusConfigForm.get(formControl).hasError('required') && this.statusConfigForm.get(formControl).touched;
  }

  ngOnInit(): void {
  }

  public submit(): void {

  }
}
