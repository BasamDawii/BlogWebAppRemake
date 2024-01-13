import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { UserRoutingModule } from './user-routing.module';
import {UserDashboardComponent} from "./components/user-dashboard/user-dashboard.component";
import {UserHomeComponent} from "./components/user-home/user-home.component";
import {MyBlogsComponent} from "./components/my-blogs/my-blogs.component";
import {CreateBlogComponent} from "./components/create-blog/create-blog.component";
import {IonicModule} from "@ionic/angular";
import {ReactiveFormsModule} from "@angular/forms";
import {SharedModule} from "../app-services/SharedModule";


@NgModule({
  declarations: [
    UserDashboardComponent,
    UserHomeComponent,
    MyBlogsComponent,
    CreateBlogComponent
  ],
  imports: [
    CommonModule,
    UserRoutingModule,
    IonicModule,
    ReactiveFormsModule,
    SharedModule
  ],
  exports: [
  ]
})
export class UserModule { }

