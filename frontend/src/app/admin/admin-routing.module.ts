import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {AdminDashboardComponent} from "./components/admin-dashboard/admin-dashboard.component";
import {RoleGuard} from "../account-management/account-guards/role.guard";
import {AdminHomeComponent} from "./components/admin-home/admin-home.component";
import {CategoriesComponent} from "./components/categories/categories.component";
import {UsersComponent} from "./components/users/users.component";
import {CreateBlogComponent} from "./components/create-blog/create-blog.component";
import {MyBlogsComponent} from "./components/my-blogs/my-blogs.component";

const routes: Routes = [
  {
    path: '',
    component: AdminDashboardComponent,
    canActivate: [RoleGuard],
    data: {expectedRole: 'Admin'},
    children: [
      {
        path: "admin-home",
        component: AdminHomeComponent
      },
      {
        path: "categories",
        component: CategoriesComponent
      },
      {
        path: "users",
        component: UsersComponent
      },
      {
        path: "create-blog",
        component: CreateBlogComponent
      },
      {
        path: "my-blogs",
        component: MyBlogsComponent
      }

    ]
  }

];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdminRoutingModule { }
