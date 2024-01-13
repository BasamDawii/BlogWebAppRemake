import {TimeAgoPipe} from "./app-pipes/TimeAgoPipe";
import {CommonModule} from "@angular/common";
import {NgModule} from "@angular/core";

@NgModule({
  declarations: [
    TimeAgoPipe
  ],
  imports: [
    CommonModule
  ],
  exports: [
    TimeAgoPipe
  ]
})
export class SharedModule {}
