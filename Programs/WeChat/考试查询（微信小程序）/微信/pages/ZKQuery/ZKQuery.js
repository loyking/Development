const app = getApp()

//下拉列表
import test from '../Template/Template.js'
//中考成绩
import ZkScore from '../Template/ZkScore.js'
//考生信息对象
import ExamineeInfo from '../Template/ExamineeInfo.js'

Page({
  data:
  {
      ShowOrHied: "none",
      checked: false,
      item: test.data,
      Zkzh: "",
      Sfz: ""
  },
  onLoad:function(){
    test.onLoad();
    this.setData({
      item:test.data
    })

    console.log(this.data.item)
  },
  selectTap() {
    test.selectTap();
    this.setData({
      item: test.data
    })
  },
  selectCity(){
    test.selectCity();
    this.setData({
      item: test.data
    })
  },
  closeTap() {
    test.closeTap();
    this.setData({
      item: test.data
    })
  },
  optionTap(e) {
    test.optionTap(e);
    this.setData({
      item: test.data
    })
    
  },
  CityoptionTap(e){
    test.CityoptionTap(e);
    this.setData({
      item: test.data
    })
  },
  ShowBox: function () {
    this.data.ShowOrHied == "none" ? this.setData({ ShowOrHied: "block" }) : this.setData({ ShowOrHied: "none" })
  },
  Agree: function () {
    this.setData({
      ShowOrHied: "none",
      checked: true
    })
  },
  scoreQuery: function () {
    if (!this.data.checked) {
      this.data.ShowOrHied == "none" ? this.setData({ ShowOrHied: "block" }) : this.setData({ ShowOrHied: "none" })
    }
    else {
      //准考证号与身份证号非空判断
      if (this.data.Zkzh != "" && this.data.Sfz != "") 
      {
        /*var para = {
          ProvinceKey: this.item.List[this.item.Selectindex].ProvinceKey,
          CityKey: this.item.List[this.item.CityIndex].CityKey,
          Zkzh: this.data.Zkzh,
          Sfz: this.data.Sfz,
          QueryType: 2
        };*/
        ExamineeInfo.data.Sfz = this.data.Sfz
        ExamineeInfo.data.Zkzh = this.data.Zkzh
        ExamineeInfo.data.ProvinceKey = this.data.item.List[this.data.item.Selectindex].ProvinceKey
        ExamineeInfo.data.CityKey = this.data.item.City[this.data.item.CityIndex].CityKey
        ExamineeInfo.data.QueryType = 2

        wx.request({
          url: 'http://localhost:9001/Home/Query',
          type: "GET",
          data: ExamineeInfo.data,
          success: function (data) {
            //检测接口是否带有正确返回值（不正确返回查询无结果页面）
            if (data.data.Existence) 
            {
              ZkScore.ChineseScore = data.data.ChineseScore
              ZkScore.MathematicsScore = data.data.MathematicsScore
              ZkScore.EnglishScore = data.data.EnglishScore
              ZkScore.HistoryScore = data.data.HistoryScore
              ZkScore.GeographyScore = data.data.GeographyScore
              ZkScore.BiologyScore = data.data.BiologyScore
              ZkScore.ChemistryScore = data.data.ChemistryScore
              ZkScore.PhysicsScore = data.data.PhysicsScore
              ZkScore.PoliticsScore = data.data.PoliticsScore
              ZkScore.SumScore = data.data.SumScore
              wx.navigateTo({
                url: '../ZKScoreResult/ZKScoreResult',
              })
            }
            else {
              wx.navigateTo({
                url: '../NoQuery/NoQuery',
              })
            }
          }
        })
      }
        else
      {

      }
    }
  },
  check: function () {
    this.setData({
      checked: true
    })
  },
  GKAdmissionsResult: function () {
    if (!this.data.checked) {
      this.data.ShowOrHied == "none" ? this.setData({ ShowOrHied: "block" }) : this.setData({ ShowOrHied: "none" })
    }
    else {
      //准考证号与身份证号非空判断
      if (this.data.Zkzh != "" && this.data.Sfz != "") 
      {
        ExamineeInfo.data.Sfz = this.data.Sfz
        ExamineeInfo.data.Zkzh = this.data.Zkzh
        ExamineeInfo.data.ProvinceKey = this.data.item.List[this.data.item.Selectindex].ProvinceKey
        ExamineeInfo.data.CityKey = this.data.item.City[this.data.item.CityIndex].CityKey
        ExamineeInfo.data.QueryType = 4

        wx.request({
          url: 'http://localhost:9001/Home/Query',
          type:"GET",
          data: ExamineeInfo.data,
          success:function(data)
          {
            if (data.data.Existence)
            {
              wx.navigateTo({
                url: '../ZKAdmissionsResults/ZKAdmissionsResults?SchoolName='+data.data.SchoolName,
              })
            }
            else
            {
              wx.navigateTo({
                url: '../NoQuery/NoQuery',
              })
            }
          }
        })

      }
    }
  },
  zkzh: function (e) {
    this.setData({
      Zkzh: e.detail.value
    })
  },
  sfz: function (e) {
    this.setData({
      Sfz: e.detail.value
    })
  }

})
