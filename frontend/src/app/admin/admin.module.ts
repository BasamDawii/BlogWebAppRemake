import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AdminRoutingModule } from './admin-routing.module';
import {AdminDashboardComponent} from "./components/admin-dashboard/admin-dashboard.component";
import {AdminHomeComponent} from "./components/admin-home/admin-home.component";
import {CategoriesComponent} from "./components/categories/categories.component";
import {UsersComponent} from "./components/users/users.component";
import {MyBlogsComponent} from "./components/my-blogs/my-blogs.component";
import {CreateBlogComponent} from "./components/create-blog/create-blog.component";
import {IonicModule} from "@ionic/angular";
import {ReactiveFormsModule} from "@angular/forms";
import {SharedModule} from "../app-services/SharedModule";


@NgModule({
  declarations: [
    AdminDashboardComponent,
    AdminHomeComponent,
    CategoriesComponent,
    UsersComponent,
    CreateBlogComponent,
    MyBlogsComponent
  ],
  imports: [
    CommonModule,
    AdminRoutingModule,
    IonicModule,
    ReactiveFormsModule,
    SharedModule,
  ]
})
export class AdminModule { }
