import { Component, Input, OnInit, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'employee-filter',
  templateUrl: './employee-filter.component.html',
  styleUrls: ['./employee-filter.component.css']
})
export class EmployeeFilterComponent implements OnInit {
  @Input()
  all: number;
  @Input()
  male: number;
  @Input()
  female: number;

  selectedRadioButtonValue: string = "all";

  @Output()
  customRadioSelectionChange: EventEmitter<string> = new EventEmitter<string>()
  constructor() { }

  ngOnInit() {
  }

  onRadioSelectionChanged() {
    //console.log(this.selectedRadioButtonValue);
    this.customRadioSelectionChange.emit(this.selectedRadioButtonValue);
  }
}
