const app = getApp();

//中考成绩
import ZkScore from '../Template/ZkScore.js'
//考生信息对象
import ExamineeInfo from '../Template/ExamineeInfo.js'

Page({
  data: {
    ChineseScore:0,
    MathematicsScore:0,
    EnglishScore:0,
    HistoryScore:0,
    GeographyScore:0,
    BiologyScore:0,
    ChemistryScore:0,
    PhysicsScore:0,
    PoliticsScore:0,
    SumScore:0
  },
  onLoad:function(){
    this.setData({
      ChineseScore: ZkScore.ChineseScore,
      MathematicsScore: ZkScore.MathematicsScore,
      EnglishScore: ZkScore.EnglishScore,
      HistoryScore: ZkScore.HistoryScore,
      GeographyScore: ZkScore.GeographyScore,
      BiologyScore: ZkScore.BiologyScore,
      ChemistryScore: ZkScore.ChemistryScore,
      PhysicsScore: ZkScore.PhysicsScore,
      PoliticsScore: ZkScore.PoliticsScore,
      SumScore: ZkScore.SumScore
    })
  },
  ContinueQuery: function () {
    wx.navigateTo({
      url: '../ZKQuery/ZKQuery',
    })
  },
  scoreQuery: function () {
    ExamineeInfo.data.QueryType = 4
    wx.request({
      url: 'http://localhost:9001/Home/Query',
      type: "GET",
      data: ExamineeInfo.data,
      success: function (data) {
        if (data.data.Existence) {
          wx.navigateTo({
            url: '../ZKAdmissionsResults/ZKAdmissionsResults?SchoolName=' + data.data.SchoolName,
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

})