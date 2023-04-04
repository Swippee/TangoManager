import { Component } from '@angular/core';
// import { writeFileSync } from 'fs'
//import { compileFromFile } from 'json-schema-to-typescript'
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'TangoManger.Client';
}
// async function generate() {
//   writeFileSync('paquet.d.ts', await compileFromFile('paquet.json'))
// }

// generate()