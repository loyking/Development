export default {
   data: {
    List:[{}],
    height: 0,
    ShengFen: "",
    Provinceshow: false,
    Cityshow: false,
    Selectindex: 0,
    CityIndex: 0,
    City:[]
  },
  onLoad: function () {
    this.data.City = this.data.List[0].CityList;
    this.CityIndex = 0;
  },
  selectTap() {
    this.data.Provinceshow=!this.data.Provinceshow
  },
  selectCity() {
    this.data.Cityshow = !this.data.Cityshow
  },
  closeTap() {
    this.data.Provinceshow = false
    this.data.Cityshow = false
  },
  optionTap(data) {
    let Index = data.currentTarget.dataset.index;//获取点击的省份下拉列表的下标
    this.data.Selectindex= Index
    this.data.City = this.data.List[Index].CityList
    this.data.Provinceshow= !this.data.Provinceshow
    this.data.CityIndex=0
  },
  CityoptionTap(data) {
    let Index = data.currentTarget.dataset.index;//获取点击的城市下拉列表的下标
    this.data.CityIndex= Index
    this.data.Cityshow=!this.data.Cityshow
  }
}