import { Component, inject, OnInit, output } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, ReactiveFormsModule, ValidatorFn, Validators } from '@angular/forms';
import { AccountService } from '../_services/account-service';
import { TextInput } from "../_forms/text-input/text-input";
import { DatePicker } from "../_forms/date-picker/date-picker";
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  imports: [ReactiveFormsModule, TextInput, DatePicker],
  templateUrl: './register.html',
  standalone: true,
  styleUrl: './register.css'
})
export class Register implements OnInit {
  private accountService = inject(AccountService);
  private formBuilder = inject(FormBuilder);
  private router = inject(Router);
  cancellationRequested = output();
  registerForm: FormGroup = new FormGroup({});
  maxDate = new Date();
  validationErrors: string[] | null = null;

  ngOnInit(): void {
    this.initForm();
    this.maxDate.setFullYear(this.maxDate.getFullYear() - 18);
  }

  initForm() {
    this.registerForm = this.formBuilder.group({
      gender:         ["male"],
      username:       ["", [Validators.required, Validators.minLength(8), Validators.maxLength(16)]],
      knownAs:        ["", [Validators.required]],
      dateOfBirth:    ["", [Validators.required]],
      city:           ["", [Validators.required]],
      country:        ["", [Validators.required]],
      password:       ["", [Validators.required, Validators.minLength(8), Validators.maxLength(64)]],
      passwordRepeat: ["", [Validators.required, this.valueMatch("password")]]
    });
    this.registerForm.controls["password"].valueChanges.subscribe({
      next: () => this.registerForm.controls["passwordRepeat"].updateValueAndValidity()
    });
  }

  valueMatch(matchTo: string): ValidatorFn {
    return (control: AbstractControl) => {
      return control.value === control.parent?.get(matchTo)?.value
        ? null
        : { ismatching: true };
    }
  }

  register() {
    var dobDateOnly = this.getDateOnly(this.registerForm.get("dateOfBirth")?.value)
    this.registerForm.patchValue({dateOfBirth: dobDateOnly});
    this.accountService.register(this.registerForm.value).subscribe({
      next: _ => this.router.navigateByUrl("/users"),
      error: error => this.validationErrors = error
    });
  }

  private getDateOnly(dob?: string) {
    if(!dob) return;
    // return dob.slice(0, 10);
    return new Date(dob).toISOString().slice(0, 10);
  }

  cancel() {
    this.cancellationRequested.emit();
  }
}
